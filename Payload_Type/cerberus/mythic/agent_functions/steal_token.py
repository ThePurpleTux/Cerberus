from mythic_container.MythicCommandBase import *
import json

class StealTokenArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="pid",
                cli_name="pid",
                display_name="pid",
                type=ParameterType.String,
                description="PID of the target process.",
                parameter_group_info=[ParameterGroupInfo(
                    required=True,
                    ui_position=0
                )]
            )
        ]

    async def parse_arguments(self):
        if len(self.command_line.strip()) == 0:
            raise Exception("Requires a pid. \nUsage: {}".format(RunCommand.help_cmd))

        if self.command_line[0] == "{":
            self.load_args_from_json_string(self.command_line)
        else:
            parts = self.command_line[0].split(" ", 1)
            self.add_arg("username", parts[0])
            self.add_arg("password", parts[1])

class StealTokenCommand(CommandBase):
    cmd = "steal_token"
    needs_admin = False
    help_cmd = "steal_token <pid>"
    description = "Steal a token from the specified process."
    version = 1
    author = "@ThePurpleTux"
    argument_class = StealTokenArguments
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