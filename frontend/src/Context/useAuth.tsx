import { createContext, useContext, useEffect, useState } from "react";
import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router-dom";
import { loginAPI, registerAPI } from "../Services/AuthService";
import { toast } from "react-toastify";
import axios from "axios";

type UserContextType = {
    user: UserProfile | null;
    token: string | null;
    registerUser: (email: string, username: string, password: string) => void;
    loginUser: (username: string, password: string) => void;
    logoutUser: () => void;
    isLoggedIn: () => boolean;
};

type Props = {
    children: React.ReactNode;
};

const UserContext = createContext<UserContextType>({} as UserContextType);

export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [token, setToken] = useState<string | null>(null);
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isReady, setIsReady] = useState(false);
    useEffect(() => {
        const user = localStorage.getItem("user");
        const token = localStorage.getItem("token");
        if (user && token) {
            setUser(JSON.parse(user));
            setToken(token);
            axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
        }
        setIsReady(true);
    }, []);
    const registerUser = async (email: string, username: string, password: string) => {
        await registerAPI(email, username, password).then((res) => {
            if (res) {
                localStorage.setItem("token", res?.data.token);
                const userObj = {
                    username: res?.data.username,
                    email: res?.data.email,
                };
                localStorage.setItem("user", JSON.stringify(user));
                setToken(res?.data.token!);
                setUser(userObj!);
                toast.success("User registered successfully");
                navigate("/search");
            }
        }).catch((err) => {
            toast.error(err.response.data.message);
        });
    };
    const loginUser = async (username: string, password: string) => {
        await loginAPI(username, password).then((res) => {
            if (res) {
                localStorage.setItem("token", res?.data.token);
                const userObj = {
                    username: res?.data.username,
                    email: res?.data.email,
                };
                localStorage.setItem("user", JSON.stringify(user));
                setToken(res?.data.token!);
                setUser(userObj!);
                toast.success("Logged in successfully");
                navigate("/search");
            }
        }).catch((err) => {
            toast.error(err.response.data.message);
        });
    };

    const isLoggedIn = () => {
        return !!user;
    };

    const logoutUser = () => {
        localStorage.removeItem("user");
        localStorage.removeItem("token");
        setUser(null);
        setToken(null);
        navigate("/");
    };

    return (
        <UserContext.Provider value={{ user, token, registerUser, loginUser, logoutUser, isLoggedIn }}>
            {isReady ? children : null}
        </UserContext.Provider>
    );
}

export const useAuth = () => useContext(UserContext);