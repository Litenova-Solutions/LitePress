type ProblemDetails = {
  status?: number;
  title?: string;
  detail?: string;
  invalidParams?: Array<{ name: string; reason: string }>;
};

export async function readApiErrorMessage(response: Response): Promise<string> {
  const body = await response.text();
  if (!body.trim()) {
    return fallbackMessage(response.status);
  }

  try {
    const problem = JSON.parse(body) as ProblemDetails;
    if (problem.detail?.trim()) {
      return problem.detail;
    }

    const validationReason = problem.invalidParams?.[0]?.reason?.trim();
    if (validationReason) {
      return validationReason;
    }

    if (problem.title?.trim()) {
      return problem.title;
    }
  } catch {
    return body.trim() || fallbackMessage(response.status);
  }

  return fallbackMessage(response.status);
}

function fallbackMessage(status: number): string {
  switch (status) {
    case 400:
      return "Validation failed.";
    case 401:
      return "You are not signed in.";
    case 403:
      return "You do not have permission to perform this action.";
    case 404:
      return "The requested resource was not found.";
    case 409:
      return "This action conflicts with the current state.";
    default:
      return "An unexpected error occurred.";
  }
}
