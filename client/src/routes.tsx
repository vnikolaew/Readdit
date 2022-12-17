import { createBrowserRouter } from "react-router-dom";
import Home from "./pages/home/Home";
import Auth from "./components/Auth";
import Login from "./pages/login/Login";
import App from "./App";
import Register from "./pages/register/Register";
import Community from "./pages/community/Community";
import CreatePost from "./pages/create-post/CreatePost";
import PostDetails from "./pages/post/PostDetails";
import UserDetails from "./pages/user/UserDetails";

export const router = createBrowserRouter([
   {
      path: "/",
      element: <App />,
      children: [
         {
            index: true,
            element: (
               <Auth redirectTo={"/login"}>
                  <Home />
               </Auth>
            ),
         },
         {
            path: "login",
            element: <Login />,
         },
         {
            path: "register",
            element: <Register />,
         },
         {
            path: "c/:communityName",
            element: <Auth redirectTo={"/login"}><Community /></Auth>,
         },
         {
            path: "post/:postId",
            element: <Auth redirectTo={"/login"}><PostDetails /></Auth>,
         },
         {
            path: "user/:userId",
            element: <Auth redirectTo={"/login"}><UserDetails /></Auth>,
         },
         {
            path: "create/post",
            element: <Auth redirectTo={"/login"}><CreatePost /></Auth>,
         },
      ],
   },
]);
