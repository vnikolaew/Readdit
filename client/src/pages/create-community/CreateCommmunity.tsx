import { Divider, Flex, Title, useMantineTheme } from "@mantine/core";
import React, { FC } from "react";
import CreateCommunityForm from "./CreateCommunityForm";

const CreateCommunity: FC = () => {
   const theme = useMantineTheme();

   return (
      <Flex
         align={"flex-start"}
         direction={"column"}
         w={"50%"}
         my={24}
         mx={"auto"}
      >
         <Title size={"h2"} fw={"semibold"} color={theme.colors.dark[9]}>
            Create a new Community
         </Title>
         <Divider
            my={2}
            orientation={"horizontal"}
            h={3}
            color={theme.colors.dark[9]}
         />
         <CreateCommunityForm />
      </Flex>
   );
};

export default CreateCommunity;
