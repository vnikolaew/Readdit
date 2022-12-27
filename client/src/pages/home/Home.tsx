import React, { FC } from "react";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import CreatePostSection from "./CreatePostSection";
import PostFeed from "./post-feed/PostFeed";
import { Divider, Flex, Group, Skeleton } from "@mantine/core";

const Home: FC = () => {
   const user = useCurrentUser();
   const { data: userInfo, isLoading } = useGetUserDetailsQuery(user?.userId);

   if (isLoading || !userInfo) {
      return (
         <Flex my={24} mx={"auto"} w={"30%"} align={"flex-start"} gap={8}>
            <Skeleton width={"100%"} height={50} circle />
            <Group style={{ flexGrow: 1 }}>
               <Skeleton width={"100%"} height={8} radius="xl" />
               <Skeleton width={"100%"} height={8} radius="xl" />
               <Skeleton width={"100%"} height={8} radius="xl" />
               <Skeleton height={8} width="70%" radius="xl" />
            </Group>
         </Flex>
      );
   }

   return (
      <Flex mt={6} direction={"column"} gap={20} align={"center"}>
         <CreatePostSection />
         <PostFeed />
         <Divider w={"50%"} orientation={"horizontal"} h={10} />
      </Flex>
   );
};

export default Home;
