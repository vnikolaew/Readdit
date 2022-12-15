import React, { FC } from "react";
import {
   Avatar,
   AvatarBadge,
   Button,
   Flex,
   Menu,
   MenuButton,
   MenuDivider,
   MenuGroup,
   MenuItem,
   MenuList,
   Text,
} from "@chakra-ui/react";
import { ChevronDownIcon, StarIcon } from "@chakra-ui/icons";
import NavigationTab from "./NavigationTab";
import { FaSignOutAlt, FaUserAlt } from "react-icons/fa";
import { BsFillPersonPlusFill } from "react-icons/bs";
import { MdOutlinePostAdd } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import { useSignOutMutation } from "../api/identity/hooks/useSignOutMutation";
import { sleep } from "../utils/sleep";
import { UserDetailsModel } from "../api/models";

interface UserDropdownMenuProps {
   user: UserDetailsModel;
}

const UserDropdownMenu: FC<UserDropdownMenuProps> = ({ user }) => {
   const navigate = useNavigate();
   const signOut = useSignOutMutation();

   return (
      <Menu>
         <MenuButton
            borderWidth={1}
            borderColor={"transparent"}
            _active={{ bgColor: "blackAlpha.400" }}
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
            <Flex bgColor={"transparent"} alignItems={"center"} gap={4}>
               <Avatar
                  borderRadius={"50%"}
                  width={30}
                  objectFit={"cover"}
                  height={30}
                  src={user!.profile!.pictureUrl!}
               >
                  <AvatarBadge
                     borderColor={"black"}
                     borderWidth={0.3}
                     boxSize={".7rem"}
                     bg={"green.500"}
                  />
               </Avatar>
               <Flex direction={"column"} alignItems={"flex-start"}>
                  <Text fontSize={16}>{user!.userName}</Text>
                  <Flex
                     alignItems={"center"}
                     justifyContent={"flex-start"}
                     gap={2}
                  >
                     <StarIcon fontSize={12} color={"yellow"} />
                     <Text>
                        Score: {user!.postsScore! + user!.commentsScore!}
                     </Text>
                  </Flex>
               </Flex>
            </Flex>
         </MenuButton>
         <MenuList
            borderRadius={10}
            py={0}
            bgColor={"blackAlpha.900"}
            border={"none"}
         >
            <MenuGroup>
               <NavigationTab
                  to={"/profile"}
                  icon={<FaUserAlt />}
                  label={"Profile"}
               />
               <NavigationTab
                  to={"/create/community"}
                  icon={<BsFillPersonPlusFill />}
                  label={"Create a community"}
               />
               <NavigationTab
                  to={"/create/post"}
                  icon={<MdOutlinePostAdd />}
                  label={"Create a post"}
               />
            </MenuGroup>
            <MenuDivider textAlign={"center"} />
            <MenuGroup>
               <MenuItem
                  borderRadius={10}
                  py={3}
                  px={6}
                  _hover={{ opacity: 0.8 }}
                  gap={6}
                  bgColor={"blackAlpha.900"}
               >
                  <FaSignOutAlt />
                  <Text
                     onClick={async () => {
                        await signOut();
                        await sleep(500);
                        navigate("/login");
                     }}
                     fontSize={14}
                  >
                     Sign Out
                  </Text>
               </MenuItem>
            </MenuGroup>
         </MenuList>
      </Menu>
   );
};

export default UserDropdownMenu;
