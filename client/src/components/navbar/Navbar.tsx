import React, { FC } from "react";
import CommunityNavigation from "./CommunityNavigation";
import UserDropdownMenu from "./UserDropdownMenu";
import ReactLink from "../Link";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import SearchBar from "./SearchBar";
import { Flex, Group, Stack, Text, useMantineTheme } from "@mantine/core";

const Navbar: FC = () => {
   const user = useCurrentUser();
   const theme = useMantineTheme();
   const { data, isLoading, error } = useGetUserDetailsQuery(user?.userId);

   if (user && error) {
      return <Text color={"red"}>{error}</Text>;
   }

   return (
      <Flex
         color={"white"}
         bg={theme.colors.gray[9]}
         sx={{ minHeight: "72px" }}
         py={8}
         px={30}
         align={"center"}
         style={{ fontSize: "20px" }}
         justify={"space-between"}
         direction={"row"}
      >
         <Group spacing={"xl"}>
            <ReactLink to={"/"}>
               <Text color={"white"} style={{ cursor: "pointer", fontSize: "24px" }}>
                  Readdit
               </Text>
            </ReactLink>
            <CommunityNavigation />
         </Group>
         {!!user?.userId && <SearchBar />}
         <Stack spacing={"xl"} fs={"17px"}>
            {!!user?.userId && !isLoading ? (
               <Flex gap={12} align={"center"}>
                  <UserDropdownMenu user={data!} />
               </Flex>
            ) : (
               <Group fz={"xl"} spacing={"xl"}>
                  <ReactLink to={"/login"}>
                     <Text color={theme.colors.gray[0]}>Login</Text>
                  </ReactLink>
                  <ReactLink to={"/register"}>
                     <Text color={theme.colors.gray[0]}>Register</Text>
                  </ReactLink>
               </Group>
            )}
         </Stack>
      </Flex>
   );
};

export default Navbar;
