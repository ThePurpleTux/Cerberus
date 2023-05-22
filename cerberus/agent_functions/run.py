from mythic_container.MythicCommandBase import *
import json

class RunArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = [
            CommandParameter(
                name="binary",
                cli_name="Binary",
                display_name="Binary",
                type=ParameterType.String,
                description="Path to a binary to execute.",
                parameter_group_info=[ParameterGroupInfo(
                    required=True,
                    ui_position=0
                )]
            ),
            CommandParameter(
                name="arguments",
                cli_name="Arguments",
                display_name="Arguments",
                type=ParameterType.String,
                default_value="",
                description="[optional] Arguments",
                parameter_group_info=[ParameterGroupInfo(
                    required=False,
                    ui_position=1
                )]
            )
        ]

    async def parse_arguments(self):
        if len(self.command_line.strip) == 0:
            raise Exception("Requires path to an executable. \nUsage: {}".format(RunCommand.help_cmd))

        if self.command_line[0] == "{":
            self.load_args_from_json_string(self.command_line)
        else:
            parts = self.command_line[0].split(" ", 1)
            self.add_arg("binary", parts[0])
            if (len(parts) > 1):
                self.add_arg("arguments", parts[1])

class RunCommand(CommandBase):
    cmd = "run"
    needs_admin = False
    help_cmd = "run <binary> <arguments>"
    description = "Execute a binary on the target system."
    version = 1
    author = "@ThePurpleTux"
    argument_class = RunArguments
    attackmapping = ["T1106", "T1218", "T1553"]
    attributes = CommandAttributes(
        suggested_command=True
    )


    async def create_go_tasking(self, taskData: PTTaskMessageAllData) -> PTTaskCreateTaskingMessageResponse:
        response = PTTaskCreateTaskingMessageResponse(
            TaskID=taskData.Task.ID,
            Success=True,
        )
        response.DisplayParams = "-Binary {} -Arguments {}".format(
            taskData.args.get_arg("binary"), taskData.args.get_arg("arguments")
        )
        return response
    
    async def process_response(self, task: PTTaskMessageAllData, response: any) -> PTTaskProcessResponseMessageResponse:
        resp = PTTaskProcessResponseMessageResponse(TaskID=task.Task.ID, Success=True)
        return resp