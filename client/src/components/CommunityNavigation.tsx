import React, { FC } from "react";
import { useCurrentUser } from "../api/common/hooks/useCurrentUser";
import { useGetMyCommunitiesQuery } from "../api/userCommunities/hooks/useGetMyCommunitiesQuery";
import { Link, useParams } from "react-router-dom";
import { Avatar, Button, Flex, Menu, MenuButton, MenuGroup, MenuItem, MenuList, Text } from "@chakra-ui/react";
import { AddIcon, ChevronDownIcon } from "@chakra-ui/icons";
import { useGetCommunityDetailsQuery } from "../api/communities";
import { AiFillHome, AiOutlineRise } from "react-icons/ai";
import { BsNewspaper } from "react-icons/bs";
import NavigationTab from "./NavigationTab";
import { log } from "../utils/logger";

const CommunityNavigation: FC = () => {
   const user = useCurrentUser();
   const { communityName } = useParams();
   const { data: communityDetails } =
      useGetCommunityDetailsQuery(communityName);
   const { data: communities } = useGetMyCommunitiesQuery(user?.userId);

   log("Current community: ", communityDetails);
   if (!user) {
      return null;
   }

   return (
      <Menu>
         <MenuButton
            borderWidth={1}
            borderColor={"transparent"}
            _active={{ bgColor: "blackAlpha.900" }}
            py={6}
            px={2}
            bgColor={"blackAlpha.400"}
            _hover={{
               bgColor: "blackAlpha.600",
               borderColor: "gray",
               borderWidth: 1,
            }}
            as={Button}
            rightIcon={<ChevronDownIcon ml={4} boxSize={6} />}
         >
            <Flex px={2} bgColor={"transparent"} alignItems={"center"} gap={4}>
               {communityDetails ? (
                  <React.Fragment>
                     <Avatar
                        borderRadius={"50%"}
                        width={4}
                        height={4}
                        objectFit={"cover"}
                        src={communityDetails.pictureUrl!}
                     />
                     <Text fontSize={14}>c/{communityDetails.name}</Text>
                  </React.Fragment>
               ) : (
                  <React.Fragment>
                     <AiFillHome />
                     <Text>Home</Text>
                  </React.Fragment>
               )}
            </Flex>
         </MenuButton>
         <MenuList bgColor={"blackAlpha.900"} border={"none"}>
            <MenuGroup fontSize={10} color={"gray"} title={"YOUR COMMUNITIES"}>
               {communities?.map((c) => (
                  <Link key={c.id} to={`/c/${c.name}`}>
                     <MenuItem
                        borderRadius={10}
                        px={4}
                        py={3}
                        _hover={{ opacity: 0.8 }}
                        gap={3}
                        bgColor={"blackAlpha.900"}
                     >
                        <Avatar
                           borderRadius={"50%"}
                           width={5}
                           height={5}
                           objectFit={"cover"}
                           src={c!.pictureUrl!}
                        />
                        <Text fontSize={14}>c/{c.name}</Text>
                     </MenuItem>
                  </Link>
               ))}
            </MenuGroup>
            <MenuGroup fontSize={10} color={"gray"} title={"FEEDS"}>
               <NavigationTab
                  to={"/"}
                  icon={<AiFillHome size={14} />}
                  label={"Home"}
               />
               <NavigationTab
                  to={"/new"}
                  icon={<BsNewspaper size={14} />}
                  label={"New"}
               />
               <NavigationTab
                  to={"/best"}
                  icon={<AiOutlineRise size={14} />}
                  label={"Popular"}
               />
            </MenuGroup>
            <MenuGroup fontSize={10} color={"gray"} title={"OTHER"}>
               <NavigationTab
                  to={"/create/post"}
                  icon={<AddIcon fontSize={14} />}
                  label={"Create a Post"}
               />
               <NavigationTab
                  to={"/create/community"}
                  icon={<AddIcon color={"red"} fontSize={14} />}
                  label={"Create a Community"}
               />
            </MenuGroup>
         </MenuList>
      </Menu>
   );
};

export default CommunityNavigation;
