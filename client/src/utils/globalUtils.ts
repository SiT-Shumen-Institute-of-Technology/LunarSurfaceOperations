import { Ref, ref, readonly } from "vue";

const isSignedIn: Ref<boolean> = ref(false);
const currentlyLoggedUsername: Ref<string | null> = ref(null);

export function useAuthState(): any {
    if (!isSignedIn.value) {
        const isThereJWT = getJWT();
        isSignedIn.value = isThereJWT  ? true : false;

        currentlyLoggedUsername.value = window.localStorage.getItem('username');
    }

    const setSignedIn = (username: string) => {
        isSignedIn.value = true;
        currentlyLoggedUsername.value = username;
    }

    const setSingnedOut = () => {
        isSignedIn.value = false;
        currentlyLoggedUsername.value = null;
    }
    
    return [
        readonly(isSignedIn),
        setSignedIn,
        setSingnedOut,
        readonly(currentlyLoggedUsername)
    ]
}

export function getJWT(): string | null {
    return window.localStorage.getItem('JWT');
}

export function setJWT(JWT: string): void {
    window.localStorage.setItem('JWT', JWT);
}

export function removeJWT(): void {
    window.localStorage.removeItem('JWT');
}
