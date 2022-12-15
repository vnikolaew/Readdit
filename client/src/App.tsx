import React from "react";
import "./App.css";
import { Box } from "@chakra-ui/react";
import { Outlet } from "react-router-dom";
import Navbar from "./components/Navbar";

function App() {
   return (
      <Box mx={"auto"} className="App">
         <Navbar />
         <Box mx={"auto"}>
            <Outlet />
         </Box>
      </Box>
   );
}

export default App;
