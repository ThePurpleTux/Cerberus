from mythic_container.MythicCommandBase import *
import json

class ShellArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="arguments",
                cli_name="Arguments",
                display_name="Arguments",
                type=ParameterType.String,
                description="Arguments to supply to cmd.exe",
                parameter_group_info=[ParameterGroupInfo(
                    required=True,
                    ui_position=0
                )]
            )
        ]

    async def parse_arguments(self):
        if len(self.command_line.strip()) == 0:
            raise Exception("Requires arguments to supply to cmd.exe. \nUsage: {}".format(RunCommand.help_cmd))

        if self.command_line[0] == "{":
            self.load_args_from_json_string(self.command_line)
        else:
            parts = self.command_line[0].split(" ", 1)
            self.add_arg("arguments", parts[0])

class ShellCommand(CommandBase):
    cmd = "shell"
    needs_admin = False
    help_cmd = "shell <arguments>"
    description = "Use cmd.exe to execute a command on the target system."
    version = 1
    author = "@ThePurpleTux"
    argument_class = ShellArguments
    attackmapping = ["T1106", "T1218", "T1553"]
    attributes = CommandAttributes(
        suggested_command=False
    )


    async def create_go_tasking(self, taskData: PTTaskMessageAllData) -> PTTaskCreateTaskingMessageResponse:
        response = PTTaskCreateTaskingMessageResponse(
            TaskID=taskData.Task.ID,
            Success=True,
        )
        response.DisplayParams = "-Arguments {}".format(
            taskData.args.get_arg("arguments")
        )
        return response
    
    async def process_response(self, task: PTTaskMessageAllData, response: any) -> PTTaskProcessResponseMessageResponse:
        resp = PTTaskProcessResponseMessageResponse(TaskID=task.Task.ID, Success=True)
        return resp