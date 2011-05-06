using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior
{
    public class BehaveDemo
    {
        //Features
            //Test Runner
                //Local and Remote execution
                // Environment agnostic (requires RemoteServer implementation for different languages (XML-RPC server).
                // (Will be) able to run tests in distributed manner using master list of test system IPs.
                // Story files not compiled. Can be changed with re-compiling.
                // Run criteria based on tags (Include and Exclude). All by default.
                // Reporting - minimal. Needs work.

        //Language Syntax
            // Blocks
                // Story            - A collection of related tests
                // Before Story     - Pre-story setup
                // After Story      - Post-story teardown
                // Criterion         - A test
                // Criterion Outline - A table driven test
                // Before Criterion  - Pre-test setup
                // After Criterion   - Post-test teardown
                // Criterion Common  - A common test sequence used at the beginning of all criteria in story
                // Test Data        - A table of test data interated over by a Criterion Outline
                // Data File        - A reference to a file of test data. Add serializers for different file types.

            // Keywords / Steps
                // Given
                // When
                // Then
                // And
                // Do
                
                // Extensibility    
                    // Can easily add new keywords to syntax without affecting test execution. 
                    // Keywords only have meaning to the test author. 
                    // Keywords have no special meaning to the test runner.

            // Tags
                // Criterion Tags used to select tests to run.

            // Comments
                // '#' is the comment token.
                // Used to comment a whole line, not partial lines.
                // Removed before parsing.

            // White space
                // Enpty Lines - Removed before parsing.
                // Leading Tabs - Removed before parsing.
                // Leading Spaces = Removed before parsing.

        //Consistent message to clients
            // Standardize our language descibing process and tools.
            // Gather language requirements for tool
            // Sync tool syntax with process language.

        //Questions and next steps
            //
    }
}
