import React from "react";
import "../App.css";
import { Outlet } from "react-router-dom";
import Navbar from "../components/navbar/Navbar";
import { Box } from "@mantine/core";

function Root() {
   return (
      <Box mx={"auto"} className="App">
         <Navbar />
         <Box mx={"auto"}>
            <Outlet />
         </Box>
      </Box>
   );
}

export default Root;
