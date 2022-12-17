import React, { FC } from "react";
import { Text } from "@chakra-ui/react";
import { WarningIcon } from "@chakra-ui/icons";
import { ApiError } from "../api/common/ApiError";
import { bool } from "yup";

interface IProps {
   show: boolean;
   errorMessage: string;
}

const ErrorMessage: FC<IProps> = ({ show, errorMessage }) => {
   return show ? (
         <Text textAlign={"start"} color={"red"}>
            <span style={{ marginRight: ".5rem" }}><WarningIcon color={"red"} fontSize={14} /></span>
            {errorMessage}
         </Text>
      ) : null;
};

export default ErrorMessage;