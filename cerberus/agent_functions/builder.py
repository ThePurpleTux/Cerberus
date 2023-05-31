import logging
import pathlib
import json
import traceback
import shutil
import os, fnmatch, tempfile, sys, asyncio
from distutils.dir_util import copy_tree

from mythic_container.PayloadBuilder import *
from mythic_container.MythicCommandBase import *
from mythic_container.MythicRPC import *


class Cerberus(PayloadType):
    name = "cerberus"
    file_extension = "exe"
    author = "@ThePurpleTux"
    supported_os = [SupportedOS.Windows]
    wrapper = False
    wrapped_payloads = []
    note = """This payload uses .NET for execution on Windows boxes."""
    supports_dynamic_loading = True
    c2_profiles = ["http"]
    mythic_encrypts = True
    translation_container = None # "myPythonTranslation"
    build_parameters = [
        BuildParameter(
            name="output_type",
            parameter_type=BuildParameterType.ChooseOne,
            choices=[ "WinExe" ],
            default_value="WinExe",
            description="Output Format"
        )
    ]
    agent_path = pathlib.Path(".") / "cerberus"
    agent_icon_path = agent_path / "agent_functions" / "cerberus.svg"
    agent_code_path = agent_path / "agent_code"

    build_steps = [
        BuildStep(step_name="Gathering Files", step_description="Making sure all commands have backing files on disk"),
        # BuildStep(step_name="Configuring", step_description="Stamping in configuration values"),
        BuildStep(step_name="Compiling", step_description="Compiling payload")
    ]

    async def build(self) -> BuildResponse:
        # Create initial response
        resp = BuildResponse(status=BuildStatus.Error)
        # resp.status = BuildStatus.Success
        success = f"Cerberus {self.uuid} successfully built"
        stdout_err = ""
        

        try:
            # Make a temp dir for building
            build_path = tempfile.TemporaryDirectory(suffix=self.uuid)
            # copy files with shutils
            copy_tree(str(self.agent_code_path), build_path.name)

            for file in get_files(build_path.name):
                if (file == "Config.cs"):

                    content = ""

                    with open(file, "r") as f:
                        content = f.read()
                        content = content.replace("__PAYLOAD_UUID_HERE__", self.uuid)

                    with open(file, "w") as f:
                        file.write(content)

            command = "nuget restore -NoCache -Force; msbuild -t:publish -p:Configuration=Release -p:Platform=\"Any CPU\" -p:SelfContained=true"
            await SendMythicRPCPayloadUpdatebuildStep(MythicRPCPayloadUpdateBuildStepMessage(
                PayloadUUID=self.uuid,
                StepName="Gathering Files",
                StepStdout="Found all files for payload",
                StepSuccess=True
            ))
            proc = await asyncio.create_subprocess_shell(command, stdout=asyncio.subprocess.PIPE, stderr=asyncio.subprocess.PIPE, cwd=build_path.name)
            stdout, stderr = await proc.communicate()

            if stdout:
                stdout_err += f'\n[stdout]\n{stdout.decode()}\n'
            if stderr:
                stdout_err += f'[stderr]\n{stderr.decode()}' + "\n" + command

            output_path = "{}/Cerberus/bin/Release/Cerberus.exe".format(build_path.name)



            if os.path.exists(output_path):
                await SendMythicRPCPayloadUpdatebuildStep(MythicRPCPayloadUpdateBuildStepMessage(
                    PayloadUUID=self.uuid,
                    StepName="Compiling",
                    StepStdout="Successfully compiled payload",
                    StepSuccess=True
                ))

            resp.payload = open(output_path, 'rb').read()
            resp.build_message = success
            resp.status = BuildStatus.Success
            resp.build_stdout = stdout_err
        
        except Exception as e:
            resp.payload = b""
            resp.status = BuildStatus.Error
            resp.build_message = "Error building payload: " + str(traceback.format_exc())
        return resp

def get_files(base_path: str) -> list[str]:
        results = []
        for root, dirs, files in os.walk(base_path):
            for name in files:
                if fnmatch.fnmatch(name, "*.cs"):
                    results.append(os.path.join(root, name))
        
        if len(results) == 0:
            raise Exception("No payload files found with extension .cs")

        return results