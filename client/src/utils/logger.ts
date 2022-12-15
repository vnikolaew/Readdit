const consoleWithMethod =
   (method: keyof Console) =>
   (...params: any[]) => {
      if (method in console && typeof console[method] === "function") {
         (console[method] as any)(...params);
      }
   };

export const log = consoleWithMethod("log");

export const warn = consoleWithMethod("warn");

export const error = consoleWithMethod("error");

export const debug = consoleWithMethod("debug");
