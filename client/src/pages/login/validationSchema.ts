import * as yup from "yup";

export const loginSchema = yup.object({
   Username: yup.string().required(),
   Password: yup.string().required(),
});
