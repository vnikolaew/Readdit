import React, { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import { QueryClientProvider } from "@tanstack/react-query";
import { __DEV__ } from "./utils/isDevMode";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { queryClient } from "./client";
import { ChakraProvider } from "@chakra-ui/react";
import theme from "./theme";
import { RouterProvider } from "react-router-dom";
import { router } from "./routes";

const root = ReactDOM.createRoot(
   document.getElementById("root") as HTMLElement
);

root.render(
   <StrictMode>
      <ChakraProvider theme={theme}>
         <QueryClientProvider client={queryClient}>
            <RouterProvider router={router} />
            {__DEV__ && <ReactQueryDevtools initialIsOpen />}
         </QueryClientProvider>
      </ChakraProvider>
   </StrictMode>
);
