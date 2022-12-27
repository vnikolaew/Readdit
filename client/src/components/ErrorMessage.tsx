import React, { FC } from "react";
import { WarningIcon } from "@chakra-ui/icons";
import { Text } from "@mantine/core";

interface IProps {
   show: boolean;
   errorMessage: string;
}

const ErrorMessage: FC<IProps> = ({ show, errorMessage }) => {
   return show ? (
      <Text fz={15} ta={"center"}>
         <span style={{ marginRight: ".5rem" }}>
            <WarningIcon color={"red"} fontSize={14} />
         </span>
         {errorMessage}
      </Text>
   ) : null;
};

export default ErrorMessage;