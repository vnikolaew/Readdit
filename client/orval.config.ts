// @ts-ignore
module.exports = {
   readdit: {
      input: {
         target: "./readdit-open-api.json",
         validation: true,
      },
      output: {
         // target: "./src/api/models.ts",
         override: {
            useTypeOverInterfaces: false,
         },
         workspace: "src/",
         schemas: "api/models",
         tslint: true,
         prettier: true,
         headers: true,
      },
      hooks: {
         afterAllFilesWrite: "prettier --write",
      },
   },
};
