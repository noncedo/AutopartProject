

CREATE VIEW [dbo].[vwILHConsolidation]
AS

SELECT
	Process.ProcessId,
	Process.[Name],
	ProcessRun.[Start],
	ProcessRun.[End],
	COUNT(ILHAudit.[ILHAuditId]) AS FileCount,
	ActiveDealerCount.ActiveDealerCount
FROM
	ILHAudit --, Dealer
INNER JOIN
	ProcessRun
	ON	ProcessRun.ProcessRunId = ILHAudit.ProcessRunId
INNER JOIN
	Process
	ON	Process.ProcessId = ProcessRun.ProcessRunId
JOIN
	(
		SELECT 
			COUNT(Dealer.DealerId) AS ActiveDealerCount
		FROM
			Dealer
		WHERE
			Dealer.IsActive = 1
	) ActiveDealerCount
	ON	ActiveDealerCount.ActiveDealerCount IS NOT NULL
GROUP BY
	Process.ProcessId,
	Process.[Name],
	ProcessRun.[Start],
	ProcessRun.[End],
	ActiveDealerCount.ActiveDealerCount