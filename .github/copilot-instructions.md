# GitHub Copilot Instructions for RimWorld Mod: Snow Covers All

## Mod Overview and Purpose

**Mod Name:** Snow Covers All  
**Description:**  
In this mod, loose items left out in the open without roof coverage are hidden under snow. Pawns can recover these items by shoveling away the snow or waiting for it to melt. The mod allows players to customize the snow depth and whether items continue to deteriorate under snow. Additionally, notifications can be set up to alert players when items are found again. This mod also integrates with the Sandstorms mod, enhancing the importance of managing sand buildup.

## Key Features and Systems

1. **Snow Covering Items:**
   - Items left without roof coverage are hidden under snow.
   - Pawns can recover items by shoveling or waiting for the snow to melt.

2. **Customizable Options:**
   - Set the snow depth and deterioration effects for items under snow.
   - Enable or disable notifications when items are found again.

3. **Integration with Sandstorms Mod:**
   - Works seamlessly with the Sandstorms mod by NightmareCorporation.
   - Enhances the need for pawns to manage sand buildup in addition to snow.

## Coding Patterns and Conventions

1. **C# Coding Patterns:**
   - Use meaningful variable and method names.
   - Follow the RimWorld coding conventions, which include:
     - Proper use of encapsulation.
     - Clear separation of concerns.
     - Utilization of interfaces and abstract classes where appropriate.

2. **XML Integration:**
   - Use XML definitions for mod configuration and settings.
   - Ensure that XML files are well-structured and maintainable.

3. **Harmony Patching:**
   - Use Harmony for patching RimWorld code.
   - Follow the Harmony patching conventions, including:
     - Clear naming of patch methods.
     - Proper use of transpilers and postfixes.
     - Error handling in patch methods.

## XML Integration

1. **XML Defs:**
   - Define mod-specific settings and configurations in XML files.
   - Ensure that XML files are located in the correct directory and are named appropriately.
   - Use XML comments to explain the purpose of each setting.

2. **Mod Settings:**
   - Implement mod settings in XML files to allow players to customize the mod.
   - Ensure that these settings are accessible from the RimWorld mod settings menu.

## Harmony Patching

1. **Patch Methods:**
   - Create Harmony patches for RimWorld code to modify game behavior.
   - Use postfix patches to add functionality after existing methods.
   - Use transpiler patches for more complex modifications.

2. **Patch Naming:**
   - Name patch methods clearly to indicate their purpose.
   - Follow the convention of prefixing patch method names with the mod name.

3. **Error Handling:**
   - Implement error handling in patch methods to prevent crashes.
   - Use try-catch blocks to catch exceptions and log errors.

## Suggestions for Copilot

1. **Code Snippets:**
   - Provide code snippets for creating Harmony patches and XML definitions.
   - Include examples of how to integrate with RimWorld systems.

2. **Documentation:**
   - Provide documentation for coding patterns and conventions.
   - Include comments in code to explain complex logic and functionality.

3. **Testing:**
   - Suggest testing strategies for ensuring mod compatibility and functionality.
   - Include instructions for debugging and troubleshooting common issues.

4. **Community Engagement:**
   - Encourage community engagement by suggesting ways to contribute to the mod.
   - Provide guidelines for reporting issues and submitting feature requests.

By following these instructions, you can effectively use GitHub Copilot to enhance your modding experience for the Snow Covers All mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

