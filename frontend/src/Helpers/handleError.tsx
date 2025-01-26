import axios from "axios";
import { toast } from "react-toastify";

export const handleError = (error: any) => {
    if (axios.isAxiosError(error)) {
        var err = error.response;
        if (Array.isArray(err?.data.errors)) {
            for (const val of err?.data.errors) {
                toast.warning(val)
            }
        }
        else if (typeof err?.data.errors === 'object') {
            for (const key in err?.data.errors) {
                if (Object.prototype.hasOwnProperty.call(err?.data.errors, key)) {
                    const element = err?.data.errors[key];
                    toast.warning(element)
                }
            }
        }
        else if (err?.data) {
            toast.warning(err?.data)
        }
        else if (err?.status === 401) {
            toast.warning("Pllease login again")
            window.history.pushState({}, "LoginPage", "/login");
        } else if (err) {
            toast.warning(err?.data)
        }
    }
};
