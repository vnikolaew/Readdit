import { MenuItem, Text } from "@chakra-ui/react";
import React, { FC } from "react";
import { Link } from "react-router-dom";

interface IProps {
   to: string;
   icon: React.ReactNode;
   label: string;

}

const NavigationTab: FC<IProps> = ({ label, icon, to }) => {
   return (
      <Link to={to}>
         <MenuItem
            borderRadius={10}
            px={4}
            py={4}
            _hover={{ opacity: 0.8 }}
            gap={3}
            bgColor={"blackAlpha.900"}
         >
            {icon}
            <Text fontSize={14}>{label}</Text>
         </MenuItem>
      </Link>
   );
};

export default NavigationTab;