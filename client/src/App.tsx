import React, { FC, useState } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "./client";
import { RouterProvider } from "react-router-dom";
import { router } from "./routes";
import { __DEV__ } from "./utils/isDevMode";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { ColorScheme, ColorSchemeProvider, MantineProvider } from "@mantine/core";
import { useColorScheme } from "@mantine/hooks";
import { NotificationsProvider } from "@mantine/notifications";

const App: FC = () => {
   const preferredColorScheme = useColorScheme();
   const [colorScheme, setColorScheme] = useState<ColorScheme>(preferredColorScheme);
   const toggleColorScheme = (value: ColorScheme) =>
      setColorScheme(value || colorScheme === "dark" ? "light" : "dark");

   return (
      <ColorSchemeProvider colorScheme={colorScheme} toggleColorScheme={toggleColorScheme}>
         <MantineProvider
            theme={{ fontFamily: "Lato", colorScheme }}
            withGlobalStyles
            withNormalizeCSS
         >
            <NotificationsProvider>
               <QueryClientProvider client={queryClient}>
                  <RouterProvider router={router} />
                  {__DEV__ && <ReactQueryDevtools initialIsOpen />}
               </QueryClientProvider>
            </NotificationsProvider>
         </MantineProvider>
      </ColorSchemeProvider>
   );
};

export default App;
