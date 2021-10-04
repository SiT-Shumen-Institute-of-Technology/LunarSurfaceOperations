export interface IWorkspace {
    id: string,
    name: string,
    description?: string
}

export interface IUseWorkspaces {
    workspaces: any,
    addWorkspace: (workspace: IWorkspace) => void,
    setWorkspaces: (workspaces: IWorkspace[]) => void,
    fetchWorkspaces: () => Promise<void>
}
