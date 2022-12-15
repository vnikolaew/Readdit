import React, { FC, useEffect, useRef, useState } from "react";
import { Box, Flex, IconButton, Image, Input, Text } from "@chakra-ui/react";
import { useFormikContext } from "formik";
import { CloseIcon } from "@chakra-ui/icons";
import { log } from "../../utils/logger";

interface ImageSelectProps {
   name: string;
}

const ImageSelect: FC<ImageSelectProps> = ({ name }) => {
   const { setFieldValue } = useFormikContext();
   const [image, setImage] = useState<File | null>(null);
   const fileRef = useRef<HTMLInputElement | null>();
   const [imagePreviewUrl, setImagePreviewUrl] = useState<string | null>();

   const handleUnselectFile = () => {
      setImage(null);
      setFieldValue(name, null!);
   };

   const onFileChange: React.ChangeEventHandler<HTMLInputElement>
      = ({ target: { files } }) => {
      const image = files?.[0];
      if (!image) return;

      setImage(image);
      setFieldValue(name, image);
      log(image);
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
         width={230}
         minHeight={100}
         bgColor={"gray.200"}
         cursor={"pointer"}
         alignItems={"center"}
         justifyContent={"center"}
         borderRadius={12}
         borderWidth={5}
         paddingBlock={2}
         boxShadow={"xl"}
      >
         {imagePreviewUrl ? (
            <Box boxShadow={"lg"} position={"relative"} borderRadius={8}>
               <Image
                  alt="Select an image"
                  objectFit={"cover"}
                  cursor={"pointer"}
                  borderRadius={5}
                  boxShadow={"lg"}
                  onClick={() => fileRef.current?.click()}
                  src={imagePreviewUrl}
               />
               <IconButton
                  position={"absolute"}
                  bgColor={"gray"}
                  _hover={{ opacity: 0.8 }}
                  top={2}
                  left={2}
                  size={"xs"}
                  borderRadius={5}
                  icon={
                     <CloseIcon fontSize={12} color={"black"} />
                  }
                  onClick={handleUnselectFile}
                  aria-label={"Unselect logo"}
               />
            </Box>
         ) : (
            <Flex
               height={"100%"}
               width={"100%"}
               onClick={() => fileRef.current?.click()}
               _hover={{ opacity: 0.6 }}
               transitionDuration={"200ms"}
               alignItems={"center"}
               justifyContent={"center"}
               cursor={"pointer"}
            >
               {image && <CloseIcon fontSize={20} color={"black"} />}
               <Text
                  fontWeight={"semibold"}
                  color={"blackAlpha.700"}
                  fontSize={20}
               >
                  Select
               </Text>
            </Flex>
         )}
         <Input
            onChange={onFileChange}
            ref={fileRef as any}
            type={"file"}
            hidden
            accept={"image/*"}
         />
      </Flex>
   );
};

export default ImageSelect;
