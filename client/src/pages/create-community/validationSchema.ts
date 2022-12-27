import * as yup from "yup";

export const validationSchema = yup.object({
   Name: yup.string().required().min(3).max(50),
   Description: yup.string().max(500).required(),
   Picture: yup.mixed().test("Media", "Please upload a valid media",
      _ => true),
   Tags: yup.array().of(yup.string().required()),
   Type: yup.number().oneOf([1, 2, 3], "Please enter a valid community type"),
});
