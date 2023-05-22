from mythic_container.MythicCommandBase import *
import json

class MakeTokenArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="username",
                cli_name="username",
                display_name="username",
                type=ParameterType.String,
                description="<Domain>\<Username> of the target user.",
                parameter_group_info=[ParameterGroupInfo(
                    required=True,
                    ui_position=0
                )]
            ),
            CommandParameter(
                name="password",
                cli_name="password",
                display_name="password",
                type=ParameterType.String,
                default_value="",
                description="password for the target user.",
                parameter_group_info=[ParameterGroupInfo(
                    required=False,
                    ui_position=1
                )]
            )
        ]

    async def parse_arguments(self):
        if len(self.command_line.strip()) == 0:
            raise Exception("Requires user credentials. \nUsage: {}".format(RunCommand.help_cmd))

        if self.command_line[0] == "{":
            self.load_args_from_json_string(self.command_line)
        else:
            parts = self.command_line[0].split(" ", 1)
            self.add_arg("username", parts[0])
            self.add_arg("password", parts[1])

class MakeTokenCommand(CommandBase):
    cmd = "make_token"
    needs_admin = False
    help_cmd = "make_token <Domain>\<Username> <password>"
    description = "Create a token using the supplied credentials."
    version = 1
    author = "@ThePurpleTux"
    argument_class = MakeTokenArguments
    attackmapping = ["T1106", "T1218", "T1553"]
    attributes = CommandAttributes(
        suggested_command=True
    )


    async def create_go_tasking(self, taskData: PTTaskMessageAllData) -> PTTaskCreateTaskingMessageResponse:
        response = PTTaskCreateTaskingMessageResponse(
            TaskID=taskData.Task.ID,
            Success=True,
        )
        response.DisplayParams = "-username {} -password {}".format(
            taskData.args.get_arg("username"), taskData.args.get_arg("password")
        )
        return response
    
    async def process_response(self, task: PTTaskMessageAllData, response: any) -> PTTaskProcessResponseMessageResponse:
        resp = PTTaskProcessResponseMessageResponse(TaskID=task.Task.ID, Success=True)
        return resp