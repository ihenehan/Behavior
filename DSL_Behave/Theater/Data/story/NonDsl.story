Story: Notify Theater Patron One Week Before
As a Theatre Patron 
I will be notified when my play performance draws near 
so I will not forget.

# Dev, QA, BA?  any of those roles can add these next steps...
	
	@doneProg
	Criterion: Test that I receive an email one week before the performance
		Given RoleIs("TheaterPatron") 
		And IPurchaseATicket()
		When DateIsSevenDaysPrior()
		Then VerifyEmailReceived()
		And StepInSecondMock()