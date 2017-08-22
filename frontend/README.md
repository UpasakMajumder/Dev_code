# Intro
To work with FE part of the project, call all commands from `/frontend` directory

# Installation
- Clone the repo
- To install all dependencies run `npm i`

# Development
* To run development mode run `gulp --dev`
* Before pushing always run `gulp kentico`, it will allow CMS to see your changes
* Because of previous issue, the possibility of merge conflicts is very high. Just remove built files and run `gulp kentico` to solve it.

# Production
To build production there are 2 options:
- Run `gulp build` – build all files to `/dist` directory
- Run `gulp kentico` – build all files to `/dist` directory and copy to Kentico CMS directory **(recommended)**
