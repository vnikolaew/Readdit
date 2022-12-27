import React, { FC } from "react";
import { Link, useParams } from "react-router-dom";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import { useGetAllPostsByUserQuery } from "../../api/posts/hooks/useGetAllPostsByUserQuery";
import moment from "moment/moment";
import { getColorByScore } from "../../utils/colors";
import { BsFillFilePostFill } from "react-icons/bs";
import { FaComment } from "react-icons/fa";
import FeedPost from "../home/feed-post/FeedPost";
import PostSkeleton from "../../components/PostSkeleton";
import {
   Avatar,
   Box,
   Divider,
   Flex,
   Group,
   Image,
   Loader,
   Text,
   Title,
   useMantineTheme,
} from "@mantine/core";

const UserDetails: FC = () => {
   const { userId } = useParams();
   const theme = useMantineTheme();
   const { data: user, isLoading } = useGetUserDetailsQuery(userId);
   const { data: userPosts, isLoading: arePostsLoading } =
      useGetAllPostsByUserQuery(userId);

   if (isLoading || !user) {
      return <Loader color={theme.colors.blue[9]} size={"md"} />;
   }

   return (
      <Box>
         <Flex
            px={60}
            sx={{ position: "relative" }}
            justify={"flex-start"}
            align={"flex-end"}
            gap={4}
            bg={theme.colors.dark[6]}
            h={150}
         >
            <Avatar
               sx={(theme) => ({
                  borderWidth: 3,
                  borderRadius: "50%",
                  borderColor: theme.colors.gray[0],
                  position: "absolute",
                  bottom: "-40%",
               })}
               h={150}
               w={150}
               src={user.profile!.pictureUrl!}
            />
            <Flex align={"flex-start"} direction={"column"} ml={180} gap={1}>
               <Title fw={"normal"} color={theme.colors.gray[0]} fz={24}>
                  {user.firstName} {user.lastName}
               </Title>
               <Text
                  fw={"normal"}
                  display={"inline"}
                  px={1}
                  ta={"left"}
                  color={theme.colors.dark[2]}
                  fz={14}
               >
                  u/{user.userName!}
                  {"   "}
                  <Text span px={6}>
                     ·
                  </Text>
                  <Text span>
                     Joined {moment(Date.parse(user!.createdOn!)).fromNow()}
                  </Text>
               </Text>
               <Group my={4} spacing={12}>
                  <Text
                     fz={18}
                     color={
                        user.profile!.gender === "Male"
                           ? theme.colors.blue[9]
                           : theme.colors.pink[9]
                     }
                     pl={1}
                  >
                     {user.profile!.gender}
                  </Text>
                  <Text color={theme.colors.dark[0]} span>
                     ·
                  </Text>
                  <Box>
                     <Image
                        height={16}
                        src={`https://flagcdn.com/256x192/${user.profile!.countryCode!.toLowerCase()}.png`}
                     />
                  </Box>
               </Group>
            </Flex>
            <Group mx={48} mb={36} spacing={36}>
               <Flex
                  color={theme.colors.gray[0]}
                  align={"flex-end"}
                  direction={"column"}
                  gap={0.5}
               >
                  <Text color={theme.colors.gray[0]} fz={16} ta={"end"}>
                     Posts Score
                  </Text>
                  <Group spacing={4}>
                     <BsFillFilePostFill
                        color={getColorByScore(user.postsScore!, theme)}
                     />
                     <Text
                        fw={"bold"}
                        color={getColorByScore(user.postsScore!, theme)}
                        fz={14}
                        ta={"end"}
                     >
                        {user.postsScore}
                     </Text>
                  </Group>
               </Flex>
               <Flex
                  color={theme.colors.gray[0]}
                  align={"flex-end"}
                  direction={"column"}
                  gap={0.5}
               >
                  <Text color={theme.colors.gray[0]} fz={16} ta={"end"}>
                     Comments Score
                  </Text>
                  <Group spacing={4}>
                     <FaComment
                        color={getColorByScore(user.commentsScore!, theme)}
                     />
                     <Text
                        fw={"bold"}
                        color={getColorByScore(user.commentsScore!, theme)}
                        fz={14}
                        ta={"end"}
                     >
                        {user.commentsScore}
                     </Text>
                  </Group>
               </Flex>
            </Group>
         </Flex>
         <Flex
            align={"center"}
            w={"100%"}
            direction={"column"}
            mx={"auto"}
            mt={10}
            gap={16}
         >
            <Title>All Posts</Title>
            <Divider
               w={"40%"}
               color={theme.colors.dark[9]}
               orientation={"horizontal"}
            />
            <Box w={"40%"}>
               {arePostsLoading &&
                  Array.from({ length: 4 }).map((_, index) => (
                     <PostSkeleton key={index} />
                  ))}
            </Box>
            {!arePostsLoading &&
               userPosts &&
               userPosts?.map((post, index) => (
                  <Link to={`/post/${post.id!}`}>
                     <FeedPost key={post.id ?? index} post={post} />
                  </Link>
               ))}
         </Flex>
      </Box>
   );
};

export default UserDetails;