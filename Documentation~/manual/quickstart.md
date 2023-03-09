---
name: Quickstart
order: 100
---

# Quickstart

1. Click the green **'Use this template'** button from the template [repository](https://github.com/PhantasmicDev/Unity-Package-Template)
2. then click **'create a new repository'** and fill in your new package's repo information.

> **Note**
> Make sure the **'include all branches'** checkbox is **NOT** checked as development on this template happens on other branches. Create your own development branch after initializing your repo.

3. Head to the **'Actions'** tab, click on the **'Initialize'** workflow, then click the **'Run workflow'** dropdown and enter your **Organization and Package Name** and finally click the **'Run workflow'** button.

The workflow will initiate and once it's done, there will be recent commits that did a couple things: 
- Edited assembly definition files
  - Renamed the files
  - Edited their content
- Edited package.json with provided package and organization name.
- Edited docfx.json so generated documentation has you package and organization name.
- Removed documentation for template repository
- Removed Initialization workflow (to prevent accidentally re-initializing your repo)

You can now clone your repo into your Unity project's Packages folder and begin development on your package!