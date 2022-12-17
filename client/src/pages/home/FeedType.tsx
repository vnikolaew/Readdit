import React, { FC, PropsWithChildren } from "react";
import { Button } from "@chakra-ui/react";
import { IconType } from "react-icons";

interface IProps extends PropsWithChildren {
   isActive: boolean;
   Icon: IconType;
   onClick?: React.MouseEventHandler<HTMLButtonElement>;
}

const FeedType: FC<IProps> = ({ isActive, Icon, children, onClick }) => {
   return (
      <Button onClick={onClick} borderRadius={"full"} _hover={{ bgColor: "whiteAlpha.100" }}
              bgColor={isActive ? "whiteAlpha.100" : "transparent"}
              color={isActive ? "white" : "gray"}
              leftIcon={<Icon size={14} color={isActive ? "white" : "gray"} />}>
         {children}
      </Button>
   );
};

export default FeedType;