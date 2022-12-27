import { Group, Menu, Text, useMantineTheme } from "@mantine/core";
import React, { FC } from "react";
import Link from "../../components/Link";

interface IProps {
   to: string;
   icon: React.ReactNode;
   label: string;
}

const NavigationTab: FC<IProps> = ({ label, icon, to }) => {
   const theme = useMantineTheme();

   return (
      <Menu.Item
         sx={(_) => ({
            "&:hover": {
               backgroundColor: theme.colors.dark[6],
            },
         })}
         bg={theme.colors.dark[9]}
         color={theme.colors.dark[0]}
      >
         <Link to={to}>
            <Group p={2} color={theme.colors.dark[0]} spacing={"xl"}>
               {icon}
               <Text color={theme.colors.dark[0]} fz={14}>
                  {label}
               </Text>
            </Group>
         </Link>
      </Menu.Item>
   );
};

export default NavigationTab;
