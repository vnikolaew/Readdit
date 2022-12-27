import React, { FC } from "react";
import { ChevronDownIcon, StarIcon } from "@chakra-ui/icons";
import NavigationTab from "./NavigationTab";
import { FaSignOutAlt, FaUserAlt } from "react-icons/fa";
import { BsFillPersonPlusFill } from "react-icons/bs";
import { MdOutlinePostAdd } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import { useSignOutMutation } from "../../api/identity/hooks/useSignOutMutation";
import { sleep } from "../../utils/sleep";
import { UserDetailsModel } from "../../api/models";
import { Avatar, Flex, Group, Indicator, Menu, Text, useMantineTheme } from "@mantine/core";

interface UserDropdownMenuProps {
   user: UserDetailsModel;
}

const UserDropdownMenu: FC<UserDropdownMenuProps> = ({ user }) => {
   const navigate = useNavigate();
   const theme = useMantineTheme();
   const signOut = useSignOutMutation();

   return (
      <Menu transitionDuration={200} transition={"fade"}>
         <Menu.Target>
            <Flex
               sx={{
                  cursor: "pointer",
                  borderRadius: 4,
                  borderWidth: 1,
                  borderColor: "transparent",
                  transitionDuration: "200ms",
                  "&:hover": {
                     borderColor: theme.colors.gray[6],
                     borderWidth: 1,
                  },
               }}
               px={10}
               py={4}
               bg={"transparent"}
               justify={"space-between"}
               align={"center"}
               gap={4}
            >
               <Group spacing={"md"}>
                  <Indicator dot inline color="teal" position="bottom-end" offset={4} size={10}>
                     <Avatar radius={"xl"} size={"md"} src={user!.profile!.pictureUrl!} />
                  </Indicator>
                  <Flex direction={"column"} align={"flex-start"}>
                     <Text color={theme.colors.dark[0]} fz={14}>
                        {user!.userName}
                     </Text>
                     <Flex align={"center"} justify={"flex-start"} gap={2}>
                        <StarIcon mr={1} fontSize={10} color={"yellow"} />
                        <Text fz={16} color={theme.colors.dark[0]}>
                           Score: {user!.postsScore! + user!.commentsScore!}
                        </Text>
                     </Flex>
                  </Flex>
               </Group>
               <ChevronDownIcon color={theme.colors.dark[0]} ml={4} boxSize={20} />
            </Flex>
         </Menu.Target>
         <Menu.Dropdown style={{ border: "none" }} p={0} bg={theme.colors.dark[9]}>
            <NavigationTab to={"/profile"} icon={<FaUserAlt size={16} />} label={"Profile"} />
            <NavigationTab
               to={"/create/community"}
               icon={<BsFillPersonPlusFill size={16} />}
               label={"Create a community"}
            />
            <NavigationTab
               to={"/create/post"}
               icon={<MdOutlinePostAdd size={16} />}
               label={"Create a post"}
            />
            <Menu.Divider mx={"auto"} w={"100%"} color={theme.colors.dark[0]} ta={"center"} />
            <Menu.Item
               sx={(_) => ({
                  "&:hover": {
                     backgroundColor: theme.colors.dark[6],
                  },
               })}
            >
               <Group py={3} px={6} spacing={"lg"}>
                  <FaSignOutAlt color={theme.colors.dark[0]} />
                  <Text
                     color={theme.colors.dark[0]}
                     onClick={async () => {
                        await signOut();
                        await sleep(500);
                        navigate("/login");
                     }}
                     fz={14}
                  >
                     Sign Out
                  </Text>
               </Group>
            </Menu.Item>
         </Menu.Dropdown>
      </Menu>
   );
};

export default UserDropdownMenu;
