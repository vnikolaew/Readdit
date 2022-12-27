import React, { FC, useEffect, useRef, useState } from "react";
import { useFormikContext } from "formik";
import { CloseIcon } from "@chakra-ui/icons";
import {
   ActionIcon,
   Box,
   FileInput,
   Flex,
   Image,
   Text,
   useMantineTheme,
} from "@mantine/core";

interface ImageSelectProps {
   name: string;
}

const ImageSelect: FC<ImageSelectProps> = ({ name }) => {
   const { setFieldValue } = useFormikContext();
   const [image, setImage] = useState<File | null>(null);
   const fileRef = useRef<HTMLInputElement | null>();
   const theme = useMantineTheme();
   const [imagePreviewUrl, setImagePreviewUrl] = useState<string | null>();

   const handleUnselectFile = () => {
      setImage(null);
      setImagePreviewUrl(null);
      setFieldValue(name, null!);
   };

   const onFileChange: (payload: File | null) => void = (payload) => {
      setImage(payload);
      setFieldValue(name, payload);
   };

   useEffect(() => {
      if (!image) {
         setImagePreviewUrl(null);
         return;
      }

      const previewUrl = URL.createObjectURL(image);
      setImagePreviewUrl(previewUrl);

      return () => URL.revokeObjectURL(previewUrl);
   }, [image]);

   return (
      <Flex
         w={230}
         sx={{
            minHeight: 100,
            cursor: "pointer",
            borderRadius: 12,
            borderWidth: 5,
            paddingBlock: 2,
         }}
         bg={theme.colors.gray[2]}
         align={"center"}
         justify={"center"}
      >
         {imagePreviewUrl ? (
            <Box
               sx={{
                  position: "relative",
                  borderRadius: 8,
               }}
            >
               <Image
                  w={"100%"}
                  alt="Select an image"
                  radius={"md"}
                  fit={"cover"}
                  sx={{
                     cursor: "pointer",
                     borderRadius: 5,
                  }}
                  onClick={() => fileRef.current?.click()}
                  src={imagePreviewUrl}
               />
               <ActionIcon
                  sx={{
                     position: "absolute",
                     padding: 4,
                     top: 8,
                     left: 8,
                     "&:hover": {
                        opacity: 0.8,
                     },
                  }}
                  radius={"md"}
                  onClick={handleUnselectFile}
                  aria-label={"Unselect logo"}
                  size={"xs"}
                  bg={"transparent"}
               >
                  <CloseIcon fontSize={12} color={"black"} />
               </ActionIcon>
            </Box>
         ) : (
            <Flex
               h={"100%"}
               w={"100%"}
               sx={{
                  transitionDuration: "200ms",
                  cursor: "pointer",
                  "&:hover": {
                     opacity: 0.6,
                  },
               }}
               bg={"transparent"}
               onClick={() => fileRef.current?.click()}
               align={"center"}
               justify={"center"}
            >
               {image && <CloseIcon fontSize={20} color={"black"} />}
               <Text fw={"bold"} color={theme.colors.dark[7]} fz={18}>
                  Choose a file
               </Text>
            </Flex>
         )}
         <FileInput
            onChange={onFileChange}
            ref={fileRef as any}
            hidden
            accept={"image/*"}
         />
      </Flex>
   );
};

export default ImageSelect;
