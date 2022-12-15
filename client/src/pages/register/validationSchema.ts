import * as yup from "yup";

export const allGenders = ["Male", "Female", "Other"];

export const registerSchema = yup.object({
   // Username: yup.string().required(),
   // Password: yup.string().required(),
   // Country: yup.string().required(),
   // Email: yup.string().email().required(),
   // EmailConfirmationUrl: yup.string().required(),
   // FirstName: yup.string().required().min(3).max(50),
   // LastName: yup.string().required().min(3).max(50),
   // Gender: yup.string().oneOf(allGenders).required(),
   // ProfilePicture: yup.object().test("Profile Picture", "Please upload a valid file", x => x === null)
});
