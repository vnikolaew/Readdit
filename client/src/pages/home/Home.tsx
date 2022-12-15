import React, { FC } from "react";
import { Box, Divider, Flex, Image, Spinner } from "@chakra-ui/react";
import { useCurrentUser } from "../../api/common/hooks/useCurrentUser";
import { useGetUserDetailsQuery } from "../../api/users/hooks/useGetUserDetailsQuery";
import CreatePostSection from "./CreatePostSection";

const Home: FC = () => {
   const user = useCurrentUser();
   const { data: userInfo, isLoading } = useGetUserDetailsQuery(user?.userId);

   if (isLoading || !userInfo) {
      return <Spinner mt={10} colorScheme={"twitter"} size={"lg"} />;
   }

   return (
      <Flex mt={6} direction={"column"} gap={6} alignItems={"center"}>
         <CreatePostSection />
         <Image
            width={200}
            height={200}
            objectFit={"cover"}
            boxShadow={"lg"}
            borderRadius={"50%"}
            src={userInfo.profile!.pictureUrl!}
         />
         <Box></Box>
         <Divider width={"50%"} orientation={"horizontal"} height={10} />
      </Flex>
   );
};

export default Home;
