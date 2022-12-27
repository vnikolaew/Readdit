import { Divider, Flex, Title, useMantineTheme } from "@mantine/core";
import React, { FC } from "react";
import CreatePostForm from "./CreatePostForm";

const CreatePost: FC = () => {
   const theme = useMantineTheme();

   return (
      <Flex
         align={"flex-start"}
         direction={"column"}
         w={"50%"}
         mt={24}
         mx={"auto"}
      >
         <Title
            fz={32}
            fw={"bold"}
            color={theme.colors.dark[8]}
         >
            Create a Post
         </Title>
         <Divider
            my={12}
            size={12}
            variant={"dashed"}
            orientation={"horizontal"}
            color={theme.colors.dark[7]}
         />
         <CreatePostForm />
      </Flex>
   );
};

export default CreatePost;
