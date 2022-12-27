import React, { FC } from "react";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetMyCommunitiesQuery } from "../../api/userCommunities/hooks/useGetMyCommunitiesQuery";
import { useParams } from "react-router-dom";
import { useGetCommunityDetailsQuery } from "../../api/communities";
import { AiFillHome } from "react-icons/ai";
import { Avatar, Flex, Group, Menu, Text, useMantineTheme } from "@mantine/core";
import { BsNewspaper } from "react-icons/bs";
import NavigationTab from "./NavigationTab";
import { IoMdRocket } from "react-icons/io";
import { AddIcon, ChevronDownIcon } from "@chakra-ui/icons";
import Link from "../Link";

const CommunityNavigation: FC = () => {
   const user = useCurrentUser();
   const { communityName } = useParams();
   const { data: communityDetails } = useGetCommunityDetailsQuery(communityName);
   const { data: communities } = useGetMyCommunitiesQuery(user?.userId);
   const theme = useMantineTheme();

   if (!user) {
      return null;
   }

   return (
      <Menu
         transitionDuration={200}
         transition={"fade"}
         offset={0}
         position={"bottom-start"}
         width={200}
         shadow={"md"}
      >
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
               align={"center"}
               gap={6}
            >
               {communityDetails ? (
                  <React.Fragment>
                     <Avatar radius={"xl"} size={"md"} src={communityDetails.pictureUrl!} />
                     <Text color={theme.colors.gray[0]} fz={14}>
                        c/{communityDetails.name}
                     </Text>
                  </React.Fragment>
               ) : (
                  <React.Fragment>
                     <AiFillHome color={theme.colors.gray[0]} />
                     <Text color={theme.colors.gray[0]}>Home</Text>
                  </React.Fragment>
               )}
               <ChevronDownIcon color={theme.colors.gray[0]} ml={4} boxSize={20} />
            </Flex>
         </Menu.Target>
         <Menu.Dropdown style={{ border: "none" }} p={0} bg={theme.colors.dark[9]}>
            <Menu.Label fz={11} color={theme.colors.gray[6]}>
               YOUR COMMUNITIES
            </Menu.Label>
            {communities?.map((c) => (
               <Menu.Item
                  sx={(_) => ({
                     // borderRadius: "10px",
                     "&:hover": {
                        backgroundColor: theme.colors.dark[6],
                     },
                  })}
                  key={c.id}
                  px={12}
                  my={4}
                  py={8}
                  bg={theme.colors.dark[9]}
               >
                  <Link to={`/c/${c.name}`}>
                     <Group spacing={"lg"}>
                        <Avatar radius={"xl"} size={"sm"} src={c!.pictureUrl!} />
                        <Text color={theme.colors.dark[0]} fz={14}>
                           c/{c.name}
                        </Text>
                     </Group>
                  </Link>
               </Menu.Item>
            ))}
            <Menu.Label fz={11} color={theme.colors.gray[8]}>
               FEEDS
            </Menu.Label>
            <Menu.Item
               sx={(_) => ({
                  "&:hover": {
                     backgroundColor: theme.colors.dark[6],
                  },
               })}
               color={theme.colors.dark[0]}
               bg={theme.colors.dark[9]}
            >
               <Link to={"/"}>
                  <Group p={2} color={theme.colors.dark[0]} spacing={"xl"}>
                     <AiFillHome color={theme.colors.dark[0]} size={16} />
                     <Text>Home</Text>
                  </Group>
               </Link>
            </Menu.Item>
            <NavigationTab to={"/new"} icon={<BsNewspaper size={14} />} label={"New"} />
            <NavigationTab to={"/best"} icon={<IoMdRocket size={14} />} label={"Best"} />
            <Menu.Label fz={11} color={"gray"}>
               OTHER
            </Menu.Label>
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
         </Menu.Dropdown>
      </Menu>
   );
};

export default CommunityNavigation;
