import { extendTheme, type ThemeConfig } from "@chakra-ui/react";

const config: ThemeConfig = {
   initialColorMode: "dark",
   useSystemColorMode: false,
};

const theme = extendTheme(
   { config },
   {
      styles: {
         global: {
            html: { fontFamily: "Lato" },
            body: { fontFamily: "Lato" },
         },
      },
   }
);

export default theme;
