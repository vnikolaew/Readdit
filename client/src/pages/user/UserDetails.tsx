import React, { FC } from "react";
import { Link, useParams } from "react-router-dom";
import { Avatar, Box, Divider, Flex, Heading, HStack, Image, Spinner, Text, VStack } from "@chakra-ui/react";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import { useGetAllPostsByUserQuery } from "../../api/posts/hooks/useGetAllPostsByUserQuery";
import moment from "moment/moment";
import { getColorByScore } from "../../utils/colors";
import { BsFillFilePostFill } from "react-icons/bs";
import { FaComment } from "react-icons/fa";
import FeedPost from "../home/feed-post/FeedPost";
import PostSkeleton from "../../components/PostSkeleton";

const UserDetails: FC = () => {
   const { userId } = useParams();
   const { data: user, isError, isLoading } = useGetUserDetailsQuery(userId);
   const { data: userPosts, isLoading: arePostsLoading } = useGetAllPostsByUserQuery(userId);

   if (isLoading || !user) {
      return <Spinner colorScheme={"facebook"} size={"md"} />;
   }

   return (
      <Box>
         <Flex
            px={20}
            position={"relative"}
            justifyContent={"flex-start"}
            alignItems={"flex-end"}
            gap={4}
            bgColor={"blackAlpha.700"}
            height={150}>
            <Avatar
               borderRadius={"50%"}
               borderWidth={3}
               borderColor={"white"}
               boxSize={140}
               position={"absolute"}
               objectFit={"cover"}
               bottom={"-30%"}
               src={user.profile!.pictureUrl!} />
            <Flex alignItems={"flex-start"} direction={"column"} ml={160} gap={1}>
               <Heading
                  fontWeight={"medium"}
                  color={"white"}
                  fontSize={24}>
                  {user.firstName} {user.lastName}
               </Heading>
               <Text
                  fontWeight={"medium"}
                  display={"inline"}
                  px={1}
                  textAlign={"left"}
                  color={"black"}
                  fontSize={16}>
                  u/{user.userName!}{"   "}
                  <Text px={2} display={"inline"}>
                     ·
                  </Text>
                  <Text display={"inline"}>
                     Joined {moment(Date.parse(user!.createdOn!)).fromNow()}
                  </Text>
               </Text>
               <HStack mb={2} spacing={4}>
                  <Text fontSize={16} color={user.profile!.gender === "Male" ? "blue.500" : "pink"} pl={1}>
                     {user.profile!.gender}
                  </Text>
                  <Text display={"inline"}>
                     ·
                  </Text>
                  <Image width={6}
                         src={`https://flagcdn.com/256x192/${user.profile!.countryCode!.toLowerCase()}.png`}
                  />
               </HStack>
            </Flex>
            <HStack mx={6} mb={4} spacing={6}>
               <Flex color={"white"} alignItems={"flex-end"} direction={"column"} gap={.5}>
                  <Text fontSize={16} textAlign={"end"}>Posts Score</Text>
                  <HStack color={getColorByScore(user.postsScore!)} spacing={1}>
                     <BsFillFilePostFill />
                     <Text fontWeight={"bold"} color={getColorByScore(user.postsScore!)} fontSize={14}
                           textAlign={"end"}>
                        {user.postsScore}
                     </Text>
                  </HStack>
               </Flex>
               <Flex color={"white"} alignItems={"flex-end"} direction={"column"} gap={.5}>
                  <Text fontSize={16} textAlign={"end"}>Comments Score</Text>
                  <HStack color={getColorByScore(user.postsScore!)} spacing={1}>
                     <FaComment />
                     <Text fontWeight={"bold"} color={getColorByScore(user.commentsScore!)} fontSize={14}
                           textAlign={"end"}>
                        {user.commentsScore}
                     </Text>
                  </HStack>
               </Flex>
            </HStack>
         </Flex>
         <VStack mt={10} spacing={4}>
            <Heading>All Posts</Heading>
            <Divider width={"40%"} color={"black"} orientation={"horizontal"} />
            {arePostsLoading && Array.from({ length: 4 }).map((_, index) => (
               <PostSkeleton key={index} />
            ))}
            {!arePostsLoading && userPosts &&
               userPosts?.map((post, index) => (
                  <Link to={`/post/${post.id!}`}>
                     <FeedPost key={post.id ?? index} post={post} />
                  </Link>
               ))
            }
         </VStack>
      </Box>
   );
};

export default UserDetails;