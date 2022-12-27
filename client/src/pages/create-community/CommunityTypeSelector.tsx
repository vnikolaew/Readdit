import React, { FC } from "react";
import { BsEyeFill, BsFillPersonFill } from "react-icons/bs";
import { FaLock } from "react-icons/fa";
import { useFormikContext } from "formik";
import { PostApiCommunitiesBody } from "../../api/models";
import { Flex, Group, Radio, Text, Title, useMantineTheme } from "@mantine/core";

const CommunityTypeSelector: FC = () => {
   const { setFieldValue } = useFormikContext<PostApiCommunitiesBody>();
   const theme = useMantineTheme();

   return (
      <Radio.Group
         onChange={(value) => {
            setFieldValue("Type", Number(value));
         }}
         my={3}
         label={
            <Title fw={"normal"} fz={18}>
               Community Type:
            </Title>
         }
         offset={"md"}
         orientation={"vertical"}
         size={"sm"}
         spacing={"sm"}
         name={"Type"}
      >
         <Flex gap={8} align={"flex-start"} direction={"column"}>
            <Radio
               label={
                  <Group spacing={8}>
                     <BsFillPersonFill size={20} color={"gray"} />
                     <Text mx={0} px={0}>
                        Public
                     </Text>
                  </Group>
               }
               color={theme.colors.blue[6]}
               value={"1"}
            ></Radio>
            <Radio
               label={
                  <Group spacing={8}>
                     <BsEyeFill size={20} color={"gray"} />
                     <Text mx={0} px={0}>
                        Restricted
                     </Text>
                  </Group>
               }
               color={theme.colors.blue[6]}
               value={"2"}
            ></Radio>
            <Radio
               label={
                  <Group spacing={8}>
                     <FaLock size={20} color={"gray"} />
                     <Text mx={0} px={0}>
                        Private
                     </Text>
                  </Group>
               }
               color={theme.colors.blue[6]}
               value={"3"}
            ></Radio>
         </Flex>
      </Radio.Group>
   );
};

export default CommunityTypeSelector;
