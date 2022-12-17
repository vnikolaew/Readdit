import React, { FC } from "react";
import { Flex, HStack, Text } from "@chakra-ui/react";
import CommunityNavigation from "./CommunityNavigation";
import UserDropdownMenu from "./UserDropdownMenu";
import { Link as ReactLink } from "react-router-dom";
import { useCurrentUser } from "../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../api/users/hooks/useGetUserDetailsQuery";

const Navbar: FC = () => {
   const user = useCurrentUser();
   const { data, isLoading, error } = useGetUserDetailsQuery(user?.userId);

   if (user && error) {
      return <Text colorScheme={"red"}>{error}</Text>;
   }

   return (
      <Flex
         color={"white"}
         bgColor={"blackAlpha.900"}
         py={2}
         px={10}
         fontSize={20}
         alignItems={"center"}
         justifyContent={"space-between"}
         direction={"row"}
      >
         <HStack gap={10}>
            <ReactLink to={"/"}>
               <Text cursor={"pointer"} fontSize={24}>Readdit</Text>
            </ReactLink>
            <CommunityNavigation />
         </HStack>
         <HStack fontSize={17} gap={16}>
            {!!user?.userId && !isLoading ? (
               <Flex gap={12} alignItems={"center"}>
                  <UserDropdownMenu user={data!} />
               </Flex>
            ) : (
               <React.Fragment>
                  <ReactLink to={"/login"}>Login</ReactLink>
                  <ReactLink to={"/register"}>Register</ReactLink>
               </React.Fragment>
            )}
         </HStack>
      </Flex>
   );
};

export default Navbar;
