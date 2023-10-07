from mythic_container.MythicCommandBase import *
import json

class SpawnInjectArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="file",
                cli_name="file",
                display_name="file",
                type=ParameterType.String,
                description="B64 encoded shellcode.",
                parameter_group_info=[ParameterGroupInfo(
                    required=True,
                    ui_position=1
                )]
            ),
            CommandParameter(
                name="path",
                cli_name="path",
                display_name="path",
                type=ParameterType.String,
                description="Path to the executable to inject into.",
                parameter_group_info=[ParameterGroupInfo(
                    required=True,
                    ui_position=0
                )]
            )
        ]

    async def parse_arguments(self):
        if len(self.command_line.strip()) == 0:
            raise Exception("Requires Path and shellcode to be supplied. \nUsage: {}".format(RunCommand.help_cmd))

        if self.command_line[0] == "{":
            self.load_args_from_json_string(self.command_line)
        else:
            parts = self.command_line[0].split(" ", 1)
            self.add_arg("path", parts[0])
            self.add_arg("file", parts[1])

class SpawnInjectCommand(CommandBase):
    cmd = "spawn-inject"
    needs_admin = False
    help_cmd = "spawn-inject <pid> <B64 File>"
    description = "Inject shellcode into a remote process using EarlyBird. \nShellcode is supplied as a B64 encoded string"
    version = 1
    author = "@ThePurpleTux"
    argument_class = SpawnInjectArguments
    attackmapping = ["T1106", "T1218", "T1553"]
    attributes = CommandAttributes(
        suggested_command=False
    )


    async def create_go_tasking(self, taskData: PTTaskMessageAllData) -> PTTaskCreateTaskingMessageResponse:
        response = PTTaskCreateTaskingMessageResponse(
            TaskID=taskData.Task.ID,
            Success=True,
        )
        return response
    
    async def process_response(self, task: PTTaskMessageAllData, response: any) -> PTTaskProcessResponseMessageResponse:
        resp = PTTaskProcessResponseMessageResponse(TaskID=task.Task.ID, Success=True)
        return resp