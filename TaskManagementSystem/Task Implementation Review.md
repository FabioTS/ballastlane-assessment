# Steps Taken

## Resume session

copilot --resume=a5e8837a-8448-4505-8851-9e9b4e003bb8

## Instructions:

### Describe how you:

1. Validated the AI's suggestions
2. Corrected or improved the output, if necessary
3. Handled edge cases, authentication, or validations

### Answers:

1. I generally use interactive Copilot mode. In this case, I can review each step and make the necessary adjustments.

2. After each step, I review the generated code and ask why it was implemented that way. I also request changes and explain how to adjust that part of the code. For example:

    *Asked user Step 2 is complete: the Task model and EF Core SQLite setup are implemented and the project builds successfully. Do you want to continue to Step 3 (single controller with CRUD operations and validation)?*

    **> Review the DueDate field. I need it to be a DateTime and not DateOnly. Also adjust the column type on EF model**

3. The plan suggested the expected validations. In this case, I used required and optional fields so the GenAI understood correctly. I also prompted for analysis:

> **> Is there any edge cases that need attention or any required validations remaining? 
> Dont make any changes yet**
> 
> No required code changes right now, but a few edge cases are worth keeping in mind:
> 
> - Title: currently required and trimmed; good.
> - UserId: required and cannot be Guid.Empty; good.
> - Status: enum parsing is fine; invalid values should fail model binding/validation.
> - DueDate: optional, but if provided it should be a valid DateTime; the app currently accepts any valid DateTime value.
> - Description: optional; empty/whitespace is normalized to null.
> - Update/delete: not found behavior is already handled.
> - No extra auth/authorization checks are needed because the prompt explicitly says UserId is only an ownership/association marker, not a managed user entity.
> 
> The only thing I’d consider optional, not required, is adding a “future due-date” rule if you want business validation later. For the current requirement, no additional validation is strictly necessary.
