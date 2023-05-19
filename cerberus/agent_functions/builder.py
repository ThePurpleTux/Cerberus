import logging
import pathlib
from mythic_container.PayloadBuilder import *
from mythic_container.MythicCommandBase import *
from mythic_container.MythicRPC import *
import json


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
        BuildStep(step_name="Configuring", step_description="Stamping in configuration values"),
        BuildStep(step_name="Compiling", step_description="Compiling payload")
    ]

    async def build(self) -> BuildResponse:
        # Create initial response
        resp = BuildResponse(status=BuildStatus.Error)
        resp.status = BuildStatus.Success
        return resp