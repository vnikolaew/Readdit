import React, { FC, PropsWithChildren } from "react";
import { useCurrentUser } from "../api/common/hooks/useCurrentUser";
import { Navigate } from "react-router-dom";

interface AuthProps extends PropsWithChildren {
   redirectTo: string;
}

const Auth: FC<AuthProps> = ({ children, redirectTo }) => {
   const user = useCurrentUser();
   if (!user) {
      return <Navigate to={redirectTo} />;
   }

   return <div>{children}</div>;
};

export default Auth;
