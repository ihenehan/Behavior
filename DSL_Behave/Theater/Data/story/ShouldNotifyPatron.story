Story: Notify Theater Patron One Week Before
As a Theatre Patron 
I will be notified when my play performance draws near 
so I will not forget.

# Dev, QA, BA?  any of those roles can add these next steps...
	
	@done
	Criterion: Test that I receive an email one week before
		Given I am a Theater Patron 
		And I have purchased a ticket
		When today is seven days from the performance date
		Then I receive a reminder email