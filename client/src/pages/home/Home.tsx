import React, { FC } from "react";
import { Divider, Flex, Spinner } from "@chakra-ui/react";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import CreatePostSection from "./CreatePostSection";
import PostFeed from "./post-feed/PostFeed";

const Home: FC = () => {
   const user = useCurrentUser();
   const { data: userInfo, isLoading } = useGetUserDetailsQuery(user?.userId);

   if (isLoading || !userInfo) {
      return <Spinner mt={10} colorScheme={"twitter"} size={"lg"} />;
   }

   return (
      <Flex mt={6} direction={"column"} gap={4} alignItems={"center"}>
         <CreatePostSection />
         <PostFeed />
         <Divider width={"50%"} orientation={"horizontal"} height={10} />
      </Flex>
   );
};

export default Home;
