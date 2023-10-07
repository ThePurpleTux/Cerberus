from mythic_container.MythicCommandBase import *
import json


class ExitArguments(TaskArguments):

    def __init__(self, command_line, **kwargs):
        super().__init__(command_line, **kwargs)
        self.args = []

    
    async def parse_arguments(self):
        if len(self.command_line) > 0:
            raise Exception("Exit takes no arguments.")


class ExitCommand(CommandBase):
    cmd = "exit"
    needs_admin = False
    help_cmd = "exit"
    description = "Task agent to exit"
    version = 2
    author = "@ThePurpleTux"
    supported_ui_features = ["callback_table:exit"]
    argument_class = ExitArguments
    attributes = CommandAttributes(
        builtin = True,
        suggest_command = True
    )
    attackmapping = []

    async def create_go_tasking(self, taskData: PTTaskMessageAllData) -> PTTaskCreateTaskingMessageResponse:
        response = PTTaskCreateTaskingMessageResponse(
            TaskID=taskData.    Task.ID,
            Success=True,
        )
        return response

    async def process_response(self, task: PTTaskMessageAllData, response: any) -> PTTaskProcessResponseMessageResponse:
        resp = PTTaskProcessResponseMessageResponse(TaskID=Task.ID, Success=True)
        return resp