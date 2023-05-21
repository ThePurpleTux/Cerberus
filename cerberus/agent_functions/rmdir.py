from mythic_container.MythicCommandBase import *
import json

class RmdirArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="path",
                type=ParameterType.String,
                description="Directory to delete.",
                parameter_group_info=[ParameterGroupInfo(
                    required=True
                )]
            ),
            CommandParameter(
                name="recurse",
                type=ParameterType.Boolean,
                default_value=False,
                description="Enable recurse? (True/False)",
                parameter_group_info=[ParameterGroupInfo(
                    required=False
                )]
            )
        ]

    async def parse_arguments(self):
        # if len(self.command_line) == 0:
        #     raise Exception("Requires path to directory you want to move to. \nUsage: {}".format(CdCommand.help_cmd))
        if self.command_line[0] == "{":
            self.load_args_from_json_string(self.command_line)
        else:
            if self.command_line[0] == '"' and self.command_line[-1] == '"':
                self.command_line = self.command_line[1:-1]
            elif self.command_line[0] == "'" and self.command_line[-1] == "'":
                self.command_line = self.command_line[1:-1]    
            self.add_arg("path", self.command_line)

class RmdirCommand(CommandBase):
    cmd = "rmdir"
    needs_admin = False
    help_cmd = "rmdir <path> <recurse (True/False)>"
    description = "Delete <path> <recurse (True/False)>"
    version = 1
    author = "@ThePurpleTux"
    argument_class = RmdirArguments
    attackmapping = ["T1106"]


    async def create_go_tasking(self, taskData: PTTaskMessageAllData) -> PTTaskCreateTaskingMessageResponse:
        response = PTTaskCreateTaskingMessageResponse(
            TaskID=taskData.Task.ID,
            Success=True,
        )
        response.DisplayParams = taskData.args.get_arg("path")
        return response
    
    async def process_response(self, task: PTTaskMessageAllData, response: any) -> PTTaskProcessResponseMessageResponse:
        resp = PTTaskProcessResponseMessageResponse(TaskID=task.Task.ID, Success=True)
        return resp